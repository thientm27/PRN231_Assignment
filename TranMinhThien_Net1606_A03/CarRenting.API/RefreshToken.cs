﻿namespace CarRenting.API
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }
    }
}
