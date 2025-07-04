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

# Lambda equivalence
# Beta Reduction
# Eta Reduction
# Console Cryptic Commands implementation
