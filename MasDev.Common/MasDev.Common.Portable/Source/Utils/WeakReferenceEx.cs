
namespace MasDev.Utils
{
	public sealed class WeakReferenceEx<T> : System.WeakReference
	{
		public new T Target {
			get {
				try {
					return (T)base.Target;
				} catch {
					return default(T);
				}
			}

			set {
				base.Target = value;
			}
		}

		public WeakReferenceEx () : base (default(T))
		{
		}

		public WeakReferenceEx (T target) : base (target)
		{
		}
	}
}

