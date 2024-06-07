using Common;

namespace ProjectGallery.CustomEventArgs
{
    public class ProjectRedirectEventArgs : EventArgs
    {
        public IProjectMeta Project { get; set; }
    }
}