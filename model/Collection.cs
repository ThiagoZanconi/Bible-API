namespace MiProyectoBackend.model{
    public class Collection(int user_id, string name, DateTime created_at){
        public int user_id {get; set;} = user_id;
        public string name {get; set;} = name;
        public DateTime created_at {get; set;} = created_at;
    }

    public class Verse_Collection(string collection_name, string book_id, int chapter, int verse){
        public string collection_name {get; set;} = collection_name;
        public string book_id {get; set;} = book_id;
        public int chapter {get; set;} = chapter;
        public int verse {get; set;} = verse;
    }

}