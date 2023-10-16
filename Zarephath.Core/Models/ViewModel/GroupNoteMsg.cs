namespace Zarephath.Core.Models.ViewModel
{
    public class GroupNoteMsg
    {
        public GroupNoteMsg()
        {
            SuccessMsg = "<ul>";
            ErrorMsg = "<ul>";
        }
        public string SuccessMsg { get; set; }
        public string ErrorMsg { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
    }
}
