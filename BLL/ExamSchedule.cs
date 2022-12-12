using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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
        struct Subject
        {
            public string MaMon;
            public int order;
            public int degree;
            public int timeslot;
            public int rooms;
        }
        int noSubjects;         // Number of subjects
        Subject[] listSubjects; // List of subjects
        int[,] adjMatrix;       // Adjacency matrix (value = 0/1)
        int noRooms;            // Number of available rooms
        int chrNumber;          // Chromatic number
        private void LoadData()
        {
            // Load data from database
            mt = mt_bll.GetList("");
            pt = pt_bll.GetList();
            ct = ct_bll.GetList();

            // Load data into necessary variable to assign exam schedule
            noSubjects = mt.Length;
            noRooms = pt.Length;
            chrNumber = 0;
            listSubjects = new Subject[noSubjects];
            for (int i = 0; i < mt.Length; i++)
            {
                Subject s = new Subject();
                s.MaMon = mt[i].MaMon; s.order = i; s.degree = 0; s.timeslot = 0; s.rooms = mt[i].SoPhong;
                listSubjects[i] = s;
            }
            adjMatrix = new int[noSubjects, noSubjects];
            for (int i = 0; i < noSubjects; i++)
            {
                for (int j = 0; j < noSubjects; j++)
                {
                    // set 0 as default value of adjacency matrix
                    adjMatrix[i, j] = 0;
                }
            }
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
        private void SortSubjects_DescDegree()
        {
            Array.Sort(listSubjects, (s1, s2) => s2.degree.CompareTo(s1.degree));
        }
        private void SortSubjects_AscTimeSlot()
        {
            Array.Sort(listSubjects, (s1, s2) => s1.timeslot.CompareTo(s2.timeslot));
        }
        public bool ScheduleExam()
        {
            LoadData();
            SortSubjects_DescDegree();
            List<Subject> coloredSubjects = new List<Subject>();
            bool notAdjacent;
            int assignedRooms = 0;
            foreach (Subject s in listSubjects)
            {
                if (s.timeslot == 0)
                {
                    chrNumber++;
                    for (int i = 0; i < noSubjects; i++)
                    {
                        if (listSubjects[i].timeslot == 0 && assignedRooms + listSubjects[i].rooms <= noRooms)
                        {
                            notAdjacent = true;
                            for (int j = 0; j < coloredSubjects.Count; j++)
                            {
                                if (adjMatrix[listSubjects[i].order, coloredSubjects[j].order] == 1)
                                {
                                    notAdjacent = false; break;
                                }
                            }
                            if (notAdjacent)
                            {
                                listSubjects[i].timeslot = chrNumber;
                                coloredSubjects.Add(listSubjects[i]);
                                assignedRooms += listSubjects[i].rooms;
                            }
                        }
                    }
                }
                coloredSubjects.Clear();
                assignedRooms = 0;
            }
            if (chrNumber > ct.Length)
            {
                return false;
            }
            SortSubjects_AscTimeSlot();
            UpdateDatabase();
            return true;
        }
        private void UpdateDatabase()
        {
            // Update MonThi(MaCa)
            for (int i = 0; i < noSubjects; i++)
            {
                string maca = ct[listSubjects[i].timeslot - 1].MaCa;
                mt_bll.Update(maca, listSubjects[i].MaMon);
            }

            // Insert into PhanBoPhongThi
            for (int i = 0; i < chrNumber; i++)
            {
                MonThi[] mt_maca = mt_bll.GetList("MaCa = '" + ct[i].MaCa + "'");
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

            // Update ThamGiaThi(MaPhong)
            for (int i = 0; i < noSubjects; i++)
            {
                string mamon = listSubjects[i].MaMon;
                tgt = tgt_bll.GetList("MaMon = '" + mamon + "'");
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