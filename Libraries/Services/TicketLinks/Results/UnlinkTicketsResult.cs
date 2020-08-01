using Helpdesk.Services.Common.Results;
using Helpdesk.Services.TicketLinks.Results.Enums;

namespace Helpdesk.Services.TicketLinks.Results
{
    public class UnlinkTicketsResult : IProcessResult<TicketsUnlinkResult>
    {
        public UnlinkTicketsResult(TicketsUnlinkResult result)
        {
            Result = result;
        }

        public TicketsUnlinkResult Result { get; }
        public string Message => GetMessage();
        public int FromTicketId { get; set; }
        public int ToTicketId { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketsUnlinkResult.Unlinked => ResultMessages.Unlinked,
            TicketsUnlinkResult.TicketsNotLinked => ResultMessages.TicketsNotLinked,
            _ => Result.ToString(),
        };

        #endregion Methods
    }
}