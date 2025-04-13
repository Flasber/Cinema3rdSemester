Using System;

Namespace BioProjektModel
{
    Public Class Ticket
    {
        Public int Id { Get; Set; }
        Public String TicketType { Get; Set; }        
        Public Decimal Price { Get; Set; }
        Public bool IsDiscounted { Get; Set; }
        Public DateTime IssueDate { Get; Set; }
    }
}
