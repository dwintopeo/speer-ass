using SpeerNotes.Common;
using SpeerNotes.Db;

namespace SpeerNotes.Tests
{
    public class InMemoryDb
    {
        public static List<Note> Notes = new() {
                new()
                {
                    Id = 1,
                    CreatedBy = "oladapo" ,
                    Title = "To-do List",
                    Details = "1. Do speer assessment. 2. Submit assessment not later than 48 hours.",
                    CreatedOn = DateTime.Now
                },
                new()
                {
                    Id = 2,
                    CreatedBy = "wintope" ,
                    Title = "Unit test note",
                    Details = "Do TDD for the assessment.",
                    CreatedOn = DateTime.Now
                },
                new()
                {
                    Id = 3,
                    CreatedBy = "oladapo" ,
                    Title = "To-do List 2",
                    Details = "To-do list for Friday.",
                    CreatedOn = DateTime.Now
                },
            };

        public static List<User> Users = new() {
                new()
                {
                    Password="xxxxxxx".Crypt(),
                    UserName = "oladapo",
                    CreatedOn = DateTime.Now,
                },
                new()
                {
                    Password="xxxxxxx".Crypt(),
                    UserName = "wintope",
                    CreatedOn = DateTime.Now,
                }
            };

        public static List<SharedNote> SharedNotes = new() {
                new()
                {
                    Id=1,
                    SharedBy = "oladapo",
                    SharedOn = DateTime.Now,
                    SharedWith="wintope",
                    NoteId=1
                },
                new()
                {
                    Id=2,
                    SharedBy = "wintope",
                    SharedOn = DateTime.Now,
                    SharedWith="oladapo",
                    NoteId=2
                }
            };
    }
}