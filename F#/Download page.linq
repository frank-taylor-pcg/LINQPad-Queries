<Query Kind="FSharpProgram">
  <Namespace>System</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

let fetchUrl callback url =
  let req = WebRequest.Create(Uri(url))
  use resp = req.GetResponse()
  use stream = resp.GetResponseStream()
  use reader = new IO.StreamReader(stream)
  callback reader url

let myCallback (reader:IO.StreamReader) url =
  let html = reader.ReadToEnd()
  let html1000 = html.Substring(0, 1000)
  printfn "Downloaded %s. First 1000 is %s" url html1000
  html

// let google = fetchUrl myCallback "http://google.com"

let fetchUrl2 = fetchUrl myCallback

let sites = ["http://google.com"; "http://microsoft.com"; "http://stackoverflow.com"]

sites |> List.map fetchUrl2

  