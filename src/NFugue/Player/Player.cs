using NFugue.Patterns;
using Sanford.Multimedia.Midi;
using System.Threading;
using System.Threading.Tasks;

namespace NFugue.Player
{
    public class Player
    {
        private readonly ManagedPlayer managedPlayer = new ManagedPlayer();

        public Sequence GetSequence(params IPatternProducer[] patternProducers)
        {
            return GetSequence(new Pattern(patternProducers));
        }

        public Sequence GetSequence(IPatternProducer patternProducer)
        {
            return GetSequence(patternProducer.GetPattern().ToString());
        }

        public Sequence GetSequence(params string[] strings)
        {
            return GetSequence(new Pattern(strings));
        }

        public Sequence GetSequence(string s)
        {
            //staccatoParser.parse(s);
            //return midiParserListener.getSequence();
            return null;
        }

        public void Play(params IPatternProducer[] patternProducers)
        {
            Play(new Pattern(patternProducers));
        }

        public void Play(IPatternProducer patternProducer)
        {
            Play(patternProducer.GetPattern().ToString());
        }

        public void Play(params string[] strings)
        {
            Play(new Pattern(strings));
        }

        public void Play(string musicString)
        {
            //Play(getSequence(string));
        }

        public void Play(Sequence sequence)
        {
            managedPlayer.Start(sequence);
            while (!managedPlayer.IsFinished)
            {
                Thread.Sleep(20);
            }
        }

        public void DelayPlay(long millisToDelay, params IPatternProducer[] patternProducers)
        {
            DelayPlay(millisToDelay, new Pattern(patternProducers));
        }

        public void DelayPlay(long millisToDelay, IPatternProducer patternProducer)
        {
            DelayPlay(millisToDelay, patternProducer.GetPattern().ToString());
        }

        public void DelayPlay(long millisToDelay, params string[] strings)
        {
            DelayPlay(millisToDelay, new Pattern(strings));
        }

        public void DelayPlay(long millisToDelay, string s)
        {
            //delayPlay(millisToDelay, GetSequence(s));
        }

        public void DelayPlay(long millisToDelay, Sequence sequence)
        {
            Task.Run(() =>
            {
                Thread.Sleep((int)millisToDelay);
                Play(sequence);
            });
        }
    }
}