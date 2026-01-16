namespace MyNet.Application.DTOs.Response
{
    public class FunctionDto
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? ParentId { get; set; }
        public List<FunctionDto> Children { get; set; } = new();
    }
}
