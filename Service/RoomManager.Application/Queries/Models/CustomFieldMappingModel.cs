using Microservice.Utility.Domain.SeedWork.CustomFieldEnum;
namespace RoomManager.Application.Queries.Models
{
    public class CustomFieldMappingModel
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public string FieldName { get; set; }
        public string OwnerType { get; set; }
        public CustomFieldDataTypeEnum DataType { get; set; }
        public InputDetailModel InputDetail { get; set; }
        public bool IsRequired { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class InputDetailModel
    {
        public InputTypeEnum InputType { get; set; }
        public string Query { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string[] Options { get; set; }
    }
}
