namespace KelpieServer.Models
{
    public class ProjectSyncResponseDto
    {
        public ProjectDto? ProjectDto { get; set; }
        public List<DatapointDto>? DatapointDtoList { get; set; }

        public void AddDatapointDto(DatapointDto datapointDto)
        {
            DatapointDtoList ??= new List<DatapointDto>();

            DatapointDtoList.Add(datapointDto);
        }
    }
}
