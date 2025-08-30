link Arc::Std::Compilation;
link Arc::Std::Array;

namespace Arc::Std::Collection
{
	public group List<T>
	{
		private field var elements: val T[];
		private field var capacity: val int;
		private field var size: val int;

		public constructor(var self: ref List<T>): ref List<T>
		{
			self.capacity = 4;
			self.size = 0;
			self.elements = CreateArray<T>(self.capacity);
		}

		public constructor(var self: ref List<T>, const data: val T[]): ref List<T>
		{
			self.capacity = GetArraySize<T>(data);
			self.size = self.capacity;
			self.elements = data;
		}

		public func append(var self: ref List<T>, const element: val T): ref none
		{
			if (self.size >= self.capacity)
			{
				self.capacity = self.capacity * 2;
				call ResizeArray<T>(self.elements, self.capacity);
			}
			call PushArray<T>(self.elements, element);
			self.size = self.size + 1;
			return none;
		}

		public func removeAt(var self: ref List<T>, const index: val int): ref none
		{
			if (index < 0 || index >= self.size)
			{
				# TODO: Error handling for out-of-bounds index
			}
			call RemoveElementFromArray<T>(self.elements, index);
			self.size = self.size - 1;
			return none;
		}

		public func clear(var self: ref List<T>): ref none
		{
			self.size = 0;
			self.capacity = 4;
			self.elements = CreateArray<T>(self.capacity);
			return none;
		}

		public func insertAt(var self: ref List<T>, const index: val int, const element: val T): ref none
		{
			if (index < 0 || index > self.size)
			{
				# TODO: Error handling for out-of-bounds index
			}
			if (self.size >= self.capacity)
			{
				self.capacity = self.capacity * 2;
				call ResizeArray<T>(self.elements, self.capacity);
			}
			call InsertElementIntoArray<T>(self.elements, index, element);
			self.size = self.size + 1;
			return none;
		}

		public func getSize(const self: ref List<T>): val int
		{
			return self.size;
		}

		public func at(const self: ref List<T>, const index: val int): val T
		{
			if (index < 0 || index >= self.size)
			{
				# TODO: Error handling for out-of-bounds index
			}
			return self.elements[index];
		}

		public func toArray(const self: ref List<T>): val T[]
		{
			var result: val T[];
			result = CreateArray<T>(self.size);
			
			var i: val int;
			for(var i : val int = 0; i < self.size; i = i + 1)
            {
                result[i] = self.elements[i];
            }
			return result;
		}

		public func indexOf(const self: ref List<T>, const element: val T): val int
		{
			for (var i : val int = 0; i < self.size; i = i + 1)
            {
                if (self.elements[i] == element)
                {
                    return i;
                }
            }
			return -1;
		}
	}
}
