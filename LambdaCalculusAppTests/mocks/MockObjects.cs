namespace LambdaCalculusAppTests.mocks;

public static class MockObjects
{
    public static readonly string[] ArithmeticTxt =
    {
        "T → λx.λy.x",
        "F → λx.λy.y",
        "IF → λb.λx.λy.b x y",
        "&& → λp.λq.p q p",
        "|| → λp.λq.p p q",
        "NOT → λp.p F T",
        "0 → λf.λx.x",
        "1 → λx.x",
        "2 → λf.λx.f (f x)",
        "3 → λf.λx.f (f (f x))",
        "4 → λf.λx.f (f (f (f x)))",
        "5 → λf.λx.f (f (f (f (f x))))",
        "6 → λf.λx.f (f (f (f (f (f x)))))",
        "7 → λf.λx.f (f (f (f (f (f (f x))))))",
        "8 → λf.λx.f (f (f (f (f (f (f (f x)))))))",
        "9 → λf.λx.f (f (f (f (f (f (f (f (f x))))))))",
        "10 → λf.λx.f (f (f (f (f (f (f (f (f (f x)))))))))",
        "++ → λn.λf.λx.f (n f x)",
        "-- → λn.λf.λx.n (λg.λh.h (g f)) (λu.x) (λu.u)",
        "+ → λn.λm.λf.λx.n f (m f x)",
        "- → λm.λn.n -- m",
        "* → λm.λn.λf.m (n f)",
        "^ → λm.λn.n m"
    };

    public const string EmptyExpressionErrorMessage = "Empty expression";

    public const string UnfinishedExpressionErrorMessage = "Unfinished expression";

    public const string InvalidCharacterErrorMessage = "Invalid character found at position";

    public const string SomethingWentWrongErrorMessage = "Something went wrong while parsing the expression";

    public const string FreeVariableErrorMessage = "Invalid free variable:";

    public const string DoubleDefinitionErrorMEssage = "There is already an expression with name";
}