module ImsccQuizzesTests

open Xunit
open CommonCartridge

[<Theory>]
[<InlineData(ImsccTestFilePaths.ModulesQuizLink1)>]
[<InlineData(ImsccTestFilePaths.ModulesQuizLink2)>]
let ``ImsccQuiz.Schema.Load loads the quiz file without throwing``(path: string) =
    // arrange, act assert
    ImsccWebLink.Schema.Load(path)
    |> ignore

[<Theory>]
[<InlineData(ImsccTestFilePaths.ModulesQuizLink1, "Quiz 1")>]
[<InlineData(ImsccTestFilePaths.ModulesQuizLink2, "Quiz 2")>]
let ``ImsccQuiz.Schema.Load reads the quiz title``(path: string, expectedTitle: string) =
    // arrange, act
    let assessment = ImsccQuiz.Schema.Load(path)
    // assert
    Assert.True(assessment.Quiz.IsSome)
    Assert.True(assessment.Quiz.Value.Title.IsSome)
    Assert.Equal(expectedTitle, assessment.Quiz.Value.Title.Value)

[<Theory>]
[<InlineData(ImsccTestFilePaths.ModulesQuizLink1, "<p>This is the content of Quiz 1.</p>")>]
[<InlineData(ImsccTestFilePaths.ModulesQuizLink2, "<p>This is the content of Quiz 2.</p>")>]
let ``ImsccQuiz.Schema.Load reads the quiz description``(path: string, expectedDescription: string) =
    // arrange, act
    let assessment = ImsccQuiz.Schema.Load(path)
    // assert
    Assert.True(assessment.Quiz.IsSome)
    Assert.True(assessment.Quiz.Value.Title.IsSome)
    Assert.Equal(expectedDescription, assessment.Quiz.Value.Description.Value)