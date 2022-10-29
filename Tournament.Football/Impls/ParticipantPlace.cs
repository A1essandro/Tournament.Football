using Tournament.Contracts;

namespace Tournament.Football
{
    public class ParticipantPlace : IParticipantPlace<Team>
    {

        public ParticipantPlace() { }

        public ParticipantPlace(Team team) => Participant = team;

        public Team Participant { get; set; }

        public override string ToString()
        {
            if (Participant != null)
                return Participant.ToString();

            return base.ToString();
        }

    }
}