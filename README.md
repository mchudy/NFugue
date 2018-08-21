# NFugue

[![Build status](https://ci.appveyor.com/api/projects/status/u25hrsi0a1d9jecd?svg=true)](https://ci.appveyor.com/project/mchudy/nfugue)
[![NuGet](https://img.shields.io/nuget/v/NFugue.svg?maxAge=60)](https://www.nuget.org/packages/NFugue/)

.NET port of [JFugue](http://jfugue.org) based on version 5.0.9.

NFugue allows you to create, play and experiment with music programatically. You can easily build music using 
elements like chords, chord progresssions and rhythms which will get transformed to MIDI for you. It also supports 
integration with some other musical formats like LilyPond or MusicXML.

The real-time music analysis and playback functionality available in JFugue is not yet implemented. 

## Download
NFugue is available on NuGet:
```
PM> Install-Package NFugue
```

## Getting started
Musical "Hello World" i.e. playing the major scale:
```csharp
using (var player = new Player())
{
    player.Play("C D E F G A B");
}
```
Music in NFugue is generally represented by patterns which can be transformed in various ways, 
for example to create some twelve-bar blues:
```csharp
Pattern pattern = new ChordProgression("I IV V")
    .Distribute("7%6")
    .AllChordsAs("$0 $0 $0 $0 $1 $1 $0 $0 $2 $1 $0 $0")
    .EachChordAs("$0ia100 $1ia80 $2ia80 $3ia80 $4ia100 $3ia80 $2ia80 $1ia80")
    .GetPattern()
    .SetInstrument(Instrument.AcousticBass)
    .SetTempo(120);
```
For more examples and a description of the Staccato markup visit [JFugue website](http://jfugue.org/). Most examples should work in NFugue with little or no modifications.
