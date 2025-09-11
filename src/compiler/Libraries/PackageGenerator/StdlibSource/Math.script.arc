namespace Arc::Std::Math
{
	public func AbsInt(const i: int): int
	{
		if (i >= 0)
		{
			return i;
		}
		else
		{
			return 0 - i;
		}
	}

	public func AbsDecimal(const d: decimal): decimal
	{
		if (d >= 0)
		{
			return d;
		}
		else
		{
			return 0 - d;
		}
	}

	public func MinInt(const a: int, const b: int): int
	{
		if(a >= b)
		{
			return b;
		}
		else
		{
			return a;
		}
	}

	public func MaxInt(const a: int, const b: int): int
	{
		if(a >= b)
		{
			return a;
		}
		else
		{
			return b;
		}
	}

	public func MinDecimal(const a: decimal, const b: decimal): decimal
	{
		if(a >= b)
		{
			return b;
		}
		else
		{
			return a;
		}
	}

	public func MaxDecimal(const a: decimal, const b: decimal): decimal
	{
		if(a >= b)
		{
			return a;
		}
		else
		{
			return b;
		}
	}

	public func Deg2Rad(const deg: decimal): decimal
	{
		return deg * 3.14159 / 180.0;
	}

	public func Rad2Deg(const rad: decimal): decimal
	{
		return rad * 180.0 / 3.14159;
	}

	public func Factorial(var n: int): int
	{
		var fac: int;
		fac = 1;
		while(n >= 1)
		{
			fac = fac * n;
			n = n - 1;
		}

		return fac;
	}

	public func Permutation(const n: int, const a: int): int
	{
		var perm: int;
		perm = 1;
		var i: int;
		i = n;
		while(i > n - a)
		{
			perm = perm * i;
			i = i - 1;
		}

		return perm;
	}

	public func Combination(const n: int, const a: int): int
	{
		var comb: int;
		comb = 1;
		var i: int = 0;
		while(i < a)
		{
			comb = comb * (n - i);
			i = i - 1;
		}

		i = 1;
		while(i <= a)
		{
			comb = comb / i;
			i = i + 1;
		}

		return comb;
	}
}
