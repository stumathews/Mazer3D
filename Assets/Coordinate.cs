namespace Assets
{
    public class Coordinate<T>
    {
        public Coordinate(T x, T y)
        {
            this.x = x;
            this.y = y;
        }
        public T GetX(){ return x;}
        public T GetY(){ return y;}
        T x, y;
    }
}
