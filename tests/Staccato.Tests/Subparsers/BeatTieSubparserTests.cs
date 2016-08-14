﻿using FluentAssertions;
using NFugue.Parser;
using Staccato.Subparsers;
using Xunit;

namespace Staccato.Tests
{
    public class BeatTimeSubparserTests : SubparserTestBase<BeatTimeSubparser>
    {
        [Fact]
        public void Should_match_at_sign()
        {
            subparser.Matches("@").Should().BeTrue();
        }

        [Fact]
        public void Track_beat_time_requested_should_be_raised()
        {
            ParseWithSubparser("@200");
            VerifyEventRaised(nameof(Parser.TrackBeatTimeRequested))
                .WithArgs<TrackBeatTimeRequestedEventArgs>(e => e.Time == 200);
        }

        [Fact]
        public void Track_beat_time_bookmarked_should_be_raised()
        {
            ParseWithSubparser("@#mark");
            VerifyEventRaised(nameof(Parser.TrackBeatTimeBookmarkRequested))
                .WithArgs<TrackBeatTimeBookmarkEventArgs>(e => e.TimeBookmarkId == "mark");
        }
    }
}