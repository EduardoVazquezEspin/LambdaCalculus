using LambdaCalculus;
using LambdaCalculus.lambda;

//var app = new ConsoleApp();

//await app.Run();

var lambda = ExpressionParser.ParseExpression("(λr.λn.(λb.λx.λy.b x y) ((λn.n (λx.λx.λy.y) λx.λy.x) n) (λf.λx.f x) ((λm.λn.λf.m (n f)) n (r ((λn.λf.λx.n (λg.λh.h (g f)) (λu.x) (λu.u)) n)))) (λf.λx.f (f (f x)))");

var result = lambda!.Compute();

var extended = new Composition(
    new Composition(
        ExpressionParser.ParseExpression("λf.λx.f (f (f x))")!, 
        ExpressionParser.ParseExpression("λn.λf.λx.f (n f x)")!),
        ExpressionParser.ParseExpression("λf.λx.x")!
    ).Compute();
    
Console.WriteLine(extended.ToString());