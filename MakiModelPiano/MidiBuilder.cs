using System;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.MusicTheory;
using java.io;

namespace MakiModelPiano
{
    internal class MidiBuilder
    {
        public static void BuildMidi()
        {

            // Build the composition
            /*var pattern = new PatternBuilder()

                .SetNoteLength(MusicalTimeSpan.Eighth.Triplet())

                .Anchor()

                .SetRootNote(Octave.Get(3).GSharp)

                .Note(Interval.Zero)  // +0  (G#3)
                .Note(Interval.Five)  // +5  (C#4)
                .Note(Interval.Eight) // +8  (E4)
                .Build();

            MidiFile midiFile = new MidiFile();
            midiFile.Chunks.Add(pattern.ToTrackChunk(TempoMap.Create(Tempo.Default, TimeSignature.Default)));
            midiFile.Write("MIDI.mid");
            */
        }

    }
}
