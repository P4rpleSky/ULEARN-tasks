using System;
using System.Text;

namespace hashes
{
	public class GhostsTask : 
		IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
		IMagic
	{
        private byte[] bytes = new byte[] { 1, 2, 3 };
        private Vector vector = new Vector(5, 5);
        private Cat cat = new Cat("Shlyopa", "Big Russian Cat", new DateTime());
        private Segment segment = new Segment(new Vector(2, 3), new Vector(8, 2));
        private Robot robot = new Robot("R2/D2");
        private Document document;

        public GhostsTask()
        {
            document = new Document("bruh", Encoding.Unicode, bytes);
        }

        public void DoMagic()
		{
            vector.Add(new Vector(2, 2));
            segment.Start.Add(new Vector(2, 2));
            cat.Rename("Ushlyopa");
            bytes[0] = 100;
            Robot.BatteryCapacity++;
        }

        Vector IFactory<Vector>.Create()
        {
            return vector;
        }

        Segment IFactory<Segment>.Create()
		{
            return segment;
		}

        Cat IFactory<Cat>.Create()
        {
            return cat;
        }

        Robot IFactory<Robot>.Create()
        {
            return robot;
        }

        Document IFactory<Document>.Create()
        {
            return document;
		}
    }
}