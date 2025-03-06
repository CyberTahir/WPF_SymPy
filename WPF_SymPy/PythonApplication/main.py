# -*- coding: utf-8 -*-

import sys
import sympy as sp
from sympy.parsing.sympy_parser import parse_expr


x = sp.Symbol('x')

if __name__ == "__main__":
	if len(sys.argv) != 3:
		print("Wrong script run: main.py <arg0> <arg1>")

	a = sys.argv[1]
	b = sys.argv[2]

	first_expr = parse_expr(a)
	second_expr = parse_expr(b)

	product_expr = first_expr * second_expr
	result = product_expr.diff(x)
	print(result)