namespace Presentation_Layer.Services
{
    public interface ITransientService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
