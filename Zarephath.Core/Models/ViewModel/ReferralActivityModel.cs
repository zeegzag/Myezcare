using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class ReferralActivityModel
    {
        public ReferralActivityModel()
        {
            ReferralActivityList = new List<ReferralActivityListModel>();

            ReferralList = new List<ReferralListModel>();
        }
        public List<ReferralActivityListModel> ReferralActivityList { get; set; }
        public List<ReferralListModel> ReferralList { get; set; }
        public List<ReferralActivityNotesModel> ReferralNotesList { get; set; }

    }

    public class ReferralActivityListModel
    {
        public int ReferralActivityMasterId { get; set; }
        public int ReferralActivityCategoryId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public bool Day1 { get; set; }
        public bool Day2 { get; set; }
        public bool Day3 { get; set; }
        public bool Day4 { get; set; }
        public bool Day5 { get; set; }
        public bool Day6 { get; set; }
        public bool Day7 { get; set; }
        public bool Day8 { get; set; }
        public bool Day9 { get; set; }
        public bool Day10 { get; set; }
        public bool Day11 { get; set; }
        public bool Day12 { get; set; }
        public bool Day13 { get; set; }
        public bool Day14 { get; set; }
        public bool Day15 { get; set; }
        public bool Day16 { get; set; }
        public bool Day17 { get; set; }
        public bool Day18 { get; set; }
        public bool Day19 { get; set; }
        public bool Day20 { get; set; }
        public bool Day21 { get; set; }
        public bool Day22 { get; set; }
        public bool Day23 { get; set; }
        public bool Day24 { get; set; }
        public bool Day25 { get; set; }
        public bool Day26 { get; set; }
        public bool Day27 { get; set; }
        public bool Day28 { get; set; }
        public bool Day29 { get; set; }
        public bool Day30 { get; set; }
        public bool Day31 { get; set; }
    }

    public class ReferralActivityNotesModel
    {
        public int ReferralActivityNoteId { get; set; }
        public int? ReferralActivityMasterId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string Initials { get; set; }
    }

}
