using System.Collections;

namespace Dimar.Gestures
{
    public interface IChainable
    {
        IEnumerator Run();
        bool Succeeded();
        bool Failed();
    }
}