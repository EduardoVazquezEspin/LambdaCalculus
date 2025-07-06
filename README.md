# Lambda Calculus Parser

This project will compile lambda calculus syntax into programmatic objects, with the intent of automating its calculus:

For example the following line
```
λx.λy.x
```
Would compile to a function
```javascript
x => y => x
```

## TO DO

1) ✅ Lambda functions override and remove previous variables with the same name. For example:`λx.(λx.x)x`Is not correctly implemented.
2) ✅ Composition is not implemented in GenericExpressionBuilder.
3) ✅ Improve error feedback.
4) ✅ There should be no throw Exception in the code. Only errors as objects.

# SIMPLIFICATIONS

### ✅ Composition is left associative

Comp(Paren(Comp(X)),Y) = Comp(X Y)   

`(x y z ...) a = x y z ... a`
 
### ✅ Lambdas extend to the right

Comp(X Paren(Lambda(V))) = Comp(X Lambda(V))

`x y z ... (λx.X) = x y z ... λx.X`

### ✅  Parenthesis of everything is unnecessary

GlobalParen(X) = X

`(x y z) = x y z`

### ✅ Parenthesis of single variable is unnecessary

Paren(V) = V

`...(x)... = ...x...`

### ✅ Parenthesis of parenthesis is unnecessary

Paren(Paren(X)) = Paren(X)

`((X)) = (X)`

### ✅ Lambda of Parenthesis is lambda

Lambda(Paren(X)) = Lambda(X)

`λx.(X) = λx.X`

# ✅ LOCAL CONTEXT

1) Add Expression? Parent to Expression class
2) Add Expression? GetLocalVariable to Expression class
   1) Default implementation Parent?.GetLocalVariable(name)
   2) Implementation for lambda is overriden to return its variable
3) Add Parent injection to constructor
4) Add Parent injection to builder

# ✅ Lambda equivalence
# Beta Reduction

Rule for beta reduction:

Composition(Lambda, x, ...(n parameters)) = Composition(Lambda(X = x), ...)

/ = Lambda(X = x) (if n == 0)

Simplify afterwardszy. y z

# ✅ Eta Reduction (Do we need to? Yes) (Can we remove do while loop?)

```λx.Y x = Y``` where Y does not depend on x

Needs a calls parameter in variable

Add as a simplifcation rule ??

Current implementation:

λ.Simplify()
loop (until λ is repeated)
   λ.EtaReduce()
   λ.Simplify()

This happens because λ.EtaReduce doesn't return a simplified form. Then the **hypothesis** is that after λ.Simplify anoter EtaReduction might be possible.

Test this by removing the loop and the second λ.Simplify, see what fails and try to get it fixed in λ.EtaReduce. Some cases are impossible since they need to modify outside of their context (but that doesn't mean they can trigger an extra λ.EtaReduce)

## Child is Composition of 3 or more

```...(Context)...λx.Comp(y1, y2, ..., x)...```

- Context is null (impossible), Parenthesis or Lambda

```λx.Comp(y1, y2, ..., x) = Comp(y1, y2, ...)```

```λy.y (λx.y y y x) λx.λy.x  = λy.y (y y y) λx.λy.x```

```λx.x λy.λx.y y y x   = λx.x λy.y y y```

Note: we need to check if parenthesis is first in composition

```λy.(λx.y y y x) λx.λy.x  = λy.y y y λx.λy.x```

Note: we need to check if last element is lambda'

```λx.(λy.y y) (λy.y y) x = (λy.y y) λy.y y```

- Context is Composition, thus we are in last position

```Comp(z1, z2, ..., λx.Comp(y1, y2, ..., x)) = Comp(z1, z2, ... Paren(y1, y2, ...))```

```λx.λy.x y λz.x y z = λx.λy.x y (x y)```

## Child is Composition of 2 elements and other element is variable

```...(Context)...λx.Comp(y, x)...```

- Context is null (impossible), Lambda, Composition

```...(Context)...λx.Comp(y, x)... = ...(Context)...y...```

```λx.λy.x y = λx.x```

```λx.x λy.x y = λx.x x```

- Context is parenthesis

```...(Context)...[λx.Comp(y, x)]... = ...(Context)...y...```

```λx.(λy.x y) x = λx.x x```

## Child is Composition of 2 elements and other element is parenthesis

```...(Context)...λx.Comp([Y], x)...```

- Context is null (impossible), Lambda, Composition, Parenthesis

```...(Context)...λx.Comp([Y], x)... = ...(Context)...Y...```

```λx.λy.(λx.x x) y = λx.λx.x x```

```λx.x λy.(λx.x x) y = λx.x λx.x x```

```λx.x (λy.(λx.x x) y) x = λx.x (λx.x x) x```

# Console Cryptic Commands implementation
