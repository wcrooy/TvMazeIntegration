namespace TvMazeIntegration.Models.Models;

public class ShowResponseModel
{
        public int MaxItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }

        public List<ShowModel> Shows { get; set; }
}