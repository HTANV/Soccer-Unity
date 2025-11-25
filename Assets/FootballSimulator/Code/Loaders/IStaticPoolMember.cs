
using Shared;
using System.Threading.Tasks;

namespace FStudio.Loaders {
    public interface IStaticPoolMember<T> where T : class {
        bool IsActive { get; }
        void MarkAsActive();
        void MarkAsDeactive();
        Task SetMember(T member);
        T Member { get; }
    }
}
