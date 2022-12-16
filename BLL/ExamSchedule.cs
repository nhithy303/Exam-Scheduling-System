using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ExamSchedule
    {
        MonThi[] mt = null;
        MonThiBLL mt_bll = new MonThiBLL();
        CaThi[] ct = null;
        CaThiBLL ct_bll = new CaThiBLL();
        ThamGiaThi[] tgt = null;
        ThamGiaThiBLL tgt_bll = new ThamGiaThiBLL();
        PhongThi[] pt = null;
        PhongThiBLL pt_bll = new PhongThiBLL();
        PhanBoPhongThiBLL pbpt_bll = new PhanBoPhongThiBLL();
        class Subject
        {
            public string MaMon { get; set; }
            public int order { get; set; }
            public int degree { get; set; }
            public int timeslot { get; set; }
            public int rooms { get; set; }
        }
        int noSubjects;             // Number of subjects
        List<Subject> listSubjects; // List of subjects
        int[,] adjMatrix;           // Adjacency matrix (value = 0/1)
        int noRooms;                // Number of available rooms
        List<bool> usedColor;       // List of color to assign exam timeslot
        private void LoadData()
        {
            // Load data of MonThi, PhongThi, CaThi from database
            mt = mt_bll.GetList("");
            pt = pt_bll.GetList();
            ct = ct_bll.GetList();

            // Set value for necessary variables
            noSubjects = mt.Length;
            noRooms = pt.Length;
            usedColor = new List<bool>(1) { true };
            listSubjects = new List<Subject>();
            for (int i = 0; i < mt.Length; i++)
            {
                Subject s = new Subject();
                s.MaMon = mt[i].MaMon; s.order = i; s.degree = 0; s.timeslot = 0; s.rooms = mt[i].SoPhong;
                listSubjects.Add(s);
            }
            
            // Initialize adjacency matrix
            adjMatrix = new int[noSubjects, noSubjects];
                // default value = 0
            for (int i = 0; i < noSubjects; i++)
            {
                for (int j = 0; j < noSubjects; j++)
                {
                    adjMatrix[i, j] = 0;
                }
            }
                // value = 1 if there exists at least one student taking those two subjects
            for (int i = 0; i < noSubjects - 1; i++)
            {
                int deg = 0;
                for (int j = i + 1; j < noSubjects; j++)
                {
                    if (tgt_bll.IsConflicting(listSubjects[i].MaMon, listSubjects[j].MaMon))
                    {
                        adjMatrix[i, j] = adjMatrix[j, i] = 1;
                        deg++;
                    }
                }
                listSubjects[i].degree = deg;
            }
        }
        public bool ScheduleExam()
        {
            LoadData();
            SortSubjects_DescDegree();
            bool finished = false;
            int count1 = -1, count2 = 0;
            while (!finished)
            {
                finished = true;
                if (count1 == count2) { break; }
                count1 = count2; count2 = 0;
                foreach (Subject s in listSubjects)
                {
                    if (s.timeslot == 0)
                    {
                        finished = false;
                        count2++;
                        int countRooms = s.rooms;
                        if (countRooms <= noRooms)
                        {
                            int currColor = GetColor(s);
                            if (currColor <= 0) { continue; }
                            s.timeslot = currColor;
                            List<Subject> coloredSubjects = new List<Subject>();
                            coloredSubjects.Add(s);
                            List<Subject> prevColorSubjects = listSubjects.FindAll(t => t.timeslot == currColor - 1);
                            foreach (Subject s1 in listSubjects)
                            {
                                if (s1.timeslot == 0)
                                {
                                    if (!IsAdjacency(s1, prevColorSubjects) && !IsAdjacency(s1, coloredSubjects))
                                    {
                                        if (countRooms + s1.rooms <= noRooms)
                                        {
                                            coloredSubjects.Add(s1);
                                            s1.timeslot = currColor;
                                            countRooms += s1.rooms;
                                        }
                                    }
                                }
                                }
                        }
                        else { return false; }
                    }
                }
            }
            if (finished)
            {
                // Exam period excesses what is assigned
                if (usedColor.Count > ct.Length) { return false; }

                // Successfully schedule exam
                SortSubjects_AscTimeSlot();
                UpdateDatabase();
            }
            return finished;
        }

        // Functions supporting to schedule exam
        private void SortSubjects_DescDegree()
        {
            listSubjects.Sort((s1, s2) => s2.degree.CompareTo(s1.degree));
        }
        private void SortSubjects_AscTimeSlot()
        {
            listSubjects.Sort((s1, s2) => s1.timeslot.CompareTo(s2.timeslot));
        }
        private int GetColor(Subject s)
        {
            int color = 0;
            List<int> index = new List<int>();
            for (int i = 0; i < usedColor.Count; i++)
            {
                if (!usedColor[i]) { index.Add(i); }
            }
            if (index.Count == 0)
            {
                usedColor.Add(false);
                index.Add(usedColor.Count - 1);
            }
            for (int i = 0; i < index.Count; i++)
            {
                color = index[i];
                if (color == 1 || !usedColor[color - 1])
                {
                    usedColor[color] = true;
                    return color;
                }
                List<Subject> prev_color = listSubjects.FindAll(t => t.timeslot == color - 1);
                if (!IsAdjacency(s, prev_color))
                {
                    usedColor[color] = true;
                    return color;
                }
                if (i + 1 == index.Count)
                {
                    usedColor.Add(false);
                    index.Add(usedColor.Count - 1);
                }
            }
            return color;
        }
        private bool IsAdjacency(Subject s, List<Subject> list)
        {
            foreach (Subject sbj in list)
            {
                if (adjMatrix[s.order, sbj.order] == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdateDatabase()
        {
            // Update MonThi(MaCa)
            for (int i = 0; i < noSubjects; i++)
            {
                string maca = ct[listSubjects[i].timeslot - 1].MaCa;
                mt_bll.UpdateMaCa(maca, listSubjects[i].MaMon);
            }

            // Insert into PhanBoPhongThi
            for (int i = 0; i < usedColor.Count; i++)
            {
                MonThi[] mt_maca = mt_bll.GetList("MaCa = '" + ct[i].MaCa + "'");
                if (mt_maca != null)
                {
                    int stt_phong = 0;
                    for (int j = 0; j < mt_maca.Length; j++)
                    {
                        int sophong = mt_maca[j].SoPhong;
                        PhanBoPhongThi pbpt = new PhanBoPhongThi();
                        pbpt.MaMon = mt_maca[j].MaMon;
                        while (sophong > 0)
                        {
                            pbpt.MaPhong = pt[stt_phong++].MaPhong;
                            pbpt_bll.Insert(pbpt);
                            sophong--;
                        }
                    }
                }
            }

            // Update ThamGiaThi(MaPhong)
            for (int i = 0; i < noSubjects; i++)
            {
                string mamon = listSubjects[i].MaMon;
                tgt = tgt_bll.GetList("MaMon = '" + mamon + "'");
                if (tgt != null)
                {
                    PhanBoPhongThi[] pbpt_mamon = pbpt_bll.GetList("MaMon = '" + mamon + "'");
                    int stt_phong = 0;
                    int succhua = pt_bll.GetList()[0].SucChua;
                    ThamGiaThi tgt_update = new ThamGiaThi();
                    tgt_update.MaMon = mamon;
                    int count = 0;
                    for (int j = 0; j < tgt.Length; j++)
                    {
                        tgt_update.MSSV = tgt[j].MSSV;
                        if (count > succhua)
                        {
                            count = 0;
                            stt_phong++;
                        }
                        count++;
                        tgt_update.MaPhong = pbpt_mamon[stt_phong].MaPhong;
                        tgt_bll.Update(tgt_update);
                    }
                }
            }
        }
    }
}