namespace SpeerNotes.Models
{
    public class BaseResponseModel
    {
        public BaseResponseModel() { }
        public BaseResponseModel(string error, string description)
        {
            AddError(error, description);
        }
        public bool Successful => !Errors.Any();
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
        public void AddError(string error) => Errors.Add(new ValidationError(error));
        public void AddError(string error, string description) => Errors.Add(new ValidationError(error, description));
    }

    public class ValidationError
    {
        public string Error { get; set; } = "";
        public string Description { get; set; } = "";
        public ValidationError() { }
        public ValidationError(string error, string description)
        {
            Error = error;
            Description = description;
        }
        public ValidationError(string error)
        {
            Error = error;
            Description = error;
        }
    }

}
