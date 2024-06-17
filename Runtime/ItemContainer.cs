namespace Utilites.Pooling
{
    public class ItemContainer<T>
	{
		private T _item;

		public bool Used { get; private set; }

		public void Caught()
		{
			Used = true;
		}

		public T Item
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
			}
		}

		public void Release()
		{
			Used = false;
		}
	}
}