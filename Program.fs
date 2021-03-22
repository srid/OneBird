// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Memstate

type Tweet =
    { Id: int
      Message: string
    }

type TwitterModel =
    { Tweets: Tweet list
    }

// Events 

type Tweeted(tweet: Tweet) =
    inherit Event()

// Commands

type PostTweet(msg: string) = 
    inherit Command<TwitterModel, int>()

    override Command.Execute(model: TwitterModel): int =
        let tweet = { Tweet.Id = 1; Message = msg }
        Command.RaiseEvent (new Tweeted(tweet))
        tweet.Id

// Query

type AllTweets() =
    inherit Query<TwitterModel, Tweet list>()

    override Query.Execute(model: TwitterModel) =
        model.Tweets

[<EntryPoint>]
let main argv =
    0 // return an integer exit code