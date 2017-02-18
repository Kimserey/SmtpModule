open System
open System.IO
open System.Net
open System.Net.Mail
open Newtonsoft.Json

type Configuration =
    { Smtp: SmtpConfiguration }
and SmtpConfiguration = 
    { Host: string
      Port: int
      UserName: string
      Password: string
      Source: string
      Recipient: string
      EnableSsl: bool }

[<EntryPoint>]
let main argv = 
    
    let cfg = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"))
    
    printfn "Start\nConfig:\n%A" cfg

    let body = """
        <h1>Email Test</h1>
        <div>Hello world!</div>
    """

    let mail = 
        new MailMessage(
            From = new MailAddress(cfg.Smtp.Source),
            Subject = "Test email",
            IsBodyHtml = true,
            Body = body
        )
    mail.To.Add(cfg.Smtp.Recipient)

    let smtpServer = 
        new SmtpClient(
            cfg.Smtp.Host,
            Port = cfg.Smtp.Port,
            Credentials = new NetworkCredential(cfg.Smtp.UserName, cfg.Smtp.Password),
            EnableSsl = cfg.Smtp.EnableSsl
        )

    smtpServer.Send(mail)

    stdin.ReadLine() |> ignore
    0
