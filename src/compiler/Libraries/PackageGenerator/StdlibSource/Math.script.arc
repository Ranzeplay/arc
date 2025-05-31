namespace Arc::Std::Math
{
	public func AbsInt(const i: val int): val int
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

	public func AbsDecimal(const d: val decimal): val decimal
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

	public func MinInt(const a: ref int, const b: ref int): ref int
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

	public func MaxInt(const a: ref int, const b: ref int): ref int
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

	public func MinDecimal(const a: ref decimal, const b: ref decimal): ref decimal
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

	public func MaxDecimal(const a: ref decimal, const b: ref decimal): ref decimal
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

	public func Deg2Rad(const deg: ref decimal): val decimal
	{
		return deg * 3.14159 / 180.0;
	}

	public func Rad2Deg(const rad: ref decimal): val decimal
	{
		return rad * 180.0 / 3.14159;
	}

	public func Factorial(var n: val int): val int
	{
		var fac: val int;
		fac = 1;
		while(n >= 1)
		{
			fac = fac * n;
			n = n - 1;
		}

		return fac;
	}

	public func Permutation(const n: val int, const a: val int): val int
	{
		var perm: val int;
		perm = 1;
		var i: val int;
		i = n;
		while(i > n - a)
		{
			perm = perm * i;
			i = i - 1;
		}

		return perm;
	}

	public func Combination(const n: val int, const a: val int): val int
	{
		var comb: val int;
		comb = 1;
		var i: val int = 0;
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
