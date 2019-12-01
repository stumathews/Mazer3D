using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class RectDetails
    {
        private Rect rect;
        public RectDetails(Rect rect)
        {
            this.rect = rect;
            
        }

        public RectDetails(int x, int y, int w, int h)
        {
            rect.x = x;
            rect.y = y;
            rect.width = w;
            rect.height = h;
        }
        public float getAx(){ return rect.x; }
        public float getAy(){ return rect.y; }
        public float getBx(){ return getAx()+rect.width;}
        public float getBy(){ return getAy();}
        public float getCx(){ return getBx();}
        public  float getCy(){ return getBy()+rect.width;}
        public float getDx(){ return getAx();}
        public float getDy(){ return getAy()+rect.width;}

        public void SetX(int x) { rect.x = x;}
        public void SetY(int y) { rect.y = +rect.width;}
        public void SetH(int h) { rect.height = h;}
        public void SetW(int w) {rect.width = w; }

        public void Init(int x, int y, int w, int h) { SetX(x); SetY(y); SetW(w);SetH(h); }

        Coordinate<float> GetA1(float intervalOfWOrH) 
        {
            return new Coordinate<float>(getAx() + (rect.width * intervalOfWOrH), getAy() );
        }
        Coordinate<float> GetB1(float intervalOfWOrH)
        {
            return new Coordinate<float>(getBx() - (rect.width * intervalOfWOrH), getBy());
        }
        Coordinate<float> GetC1(float intervalOfWOrH)	
        {
            return new Coordinate<float>(getCx() - (rect.width * intervalOfWOrH), getCy());
        }
        Coordinate<float> GetD1(float intervalOfWOrH)
        {
            return new Coordinate<float>(getDx() + (rect.width * intervalOfWOrH), getDy());
        }

        Coordinate<float> GetA2(float intervalOfWOrH) 
        {
            return new Coordinate<float>(getAx(), getAy() + (rect.width * intervalOfWOrH) );
        }
        Coordinate<float> GetB2(float intervalOfWOrH)
        {
            return new Coordinate<float>(getBx(), getBy() + (rect.width * intervalOfWOrH));
        }
        Coordinate<float> GetC2(float intervalOfWOrH)	
        {
            return new Coordinate<float>(getCx(), getCy() - (rect.width * intervalOfWOrH));
        }
        Coordinate<float> GetD2(float intervalOfWOrH)
        {
            return new Coordinate<float>(getDx(),  getDy() - (rect.width * intervalOfWOrH));
        }
    }
}
