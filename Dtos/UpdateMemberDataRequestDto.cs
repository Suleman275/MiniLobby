﻿using MiniLobby.Misc;
using System.ComponentModel.DataAnnotations;

namespace MiniLobby.Dtos {
    public class UpdateMemberDataRequestDto {
        [Required]
        public Guid RequestSenderId { get; set; }
        public Dictionary<string, DataPoint> Data { get; set; }
    }
}
