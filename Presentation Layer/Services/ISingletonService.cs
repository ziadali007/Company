namespace Presentation_Layer.Services
{
    public interface ISingletonService
    {
        public Guid Guid { get; set; }

        string GetGuid();
    }
}
