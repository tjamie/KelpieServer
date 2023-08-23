using KelpieServer.Models;

namespace KelpieServer.Mappers
{
    public class UserMapper
    {
        public User MapToEntity(UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Admin = dto.Admin,
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email ?? null
            };
        }
        public void MapToEntity(UserDto dto, ref User target)
        {
            target.Id = dto.Id;
            target.Admin = dto.Admin;
            target.Username = dto.Username;
            target.Password = dto.Password;
            target.Email = dto.Email ?? null;
        }
    }
}
