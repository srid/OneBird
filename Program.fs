// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Memstate
open Memstate.JsonNet

type Tweet =
    { Id: int
      Message: string
    }

type TwitterModel() =
    let tweets = ResizeArray()

    member this.Tweets = Seq.toList tweets

    member this.PostTweet(msg: string) = 
        let tweet = { Tweet.Id = 1; Message = msg }
        tweets.Add(tweet)
        tweet
        

// Events 

type Tweeted(tweet: Tweet) =
    inherit Event()

// Commands

type PostTweet(msg: string) = 
    inherit Command<TwitterModel, int>()

    member this.Msg: string = msg

    override Command.Execute(model: TwitterModel): int =
        let tweet = model.PostTweet msg
        Command.RaiseEvent (new Tweeted(tweet))
        tweet.Id

// Query

type AllTweets() =
    inherit Query<TwitterModel, Tweet list>()

    override Query.Execute(model: TwitterModel) =
        model.Tweets

[<EntryPoint>]
let main argv =
    // TODO: understand concepts used
    printfn "Begin"
    async {
        let! engine = Engine.Start<TwitterModel>() |> Async.AwaitTask
        let cmd = new PostTweet("Hello world")
        let! res = engine.Execute(cmd) |> Async.AwaitTask
        printfn "res: %d" res
        let query = new AllTweets()
        let! allTweets = engine.Execute(query) |> Async.AwaitTask
        printfn "%A" allTweets
        printfn "Fin."
    } |> Async.RunSynchronously
    0 // return an integer exit code