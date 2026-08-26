using COMS.Data;
using COMS.DTOs;
using COMS.Models;
<<<<<<< HEAD
using Google.Cloud.Firestore;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

namespace COMS.Services;

public class UserService : IUserService
{
<<<<<<< HEAD
    private readonly IFirestoreRepository<User> _userRepo;
    private readonly IJwtService _jwtService;

    public UserService(IFirestoreRepository<User> userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
=======
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public UserService(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        _jwtService = jwtService;
    }

    public async Task<UserResponseDto> RegisterAsync(RegisterUserDto dto)
    {
<<<<<<< HEAD
        var existing = (await _userRepo.QueryAsync(q => q.WhereEqualTo("Email", dto.Email).Limit(1))).FirstOrDefault();
=======
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (existing != null)
            throw new InvalidOperationException("Email already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Role = dto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Barangay = dto.Barangay,
            Municipality = dto.Municipality,
            Province = dto.Province,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

<<<<<<< HEAD
        await _userRepo.CreateAsync(user);
=======
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return MapToResponse(user);
    }

    public async Task<string?> LoginAsync(LoginUserDto dto)
    {
<<<<<<< HEAD
        var user = (await _userRepo.QueryAsync(q => q.WhereEqualTo("Email", dto.Email).Limit(1))).FirstOrDefault();
=======
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        if (!user.IsActive)
            return null;

        return _jwtService.GenerateToken(user);
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
<<<<<<< HEAD
        var user = await _userRepo.GetByIdAsync(id.ToString());
=======
        var user = await _context.Users.FindAsync(id);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return user == null ? null : MapToResponse(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
<<<<<<< HEAD
        var users = await _userRepo.QueryAsync(q => q.OrderByDescending("CreatedAt"));
        return users.Select(MapToResponse);
=======
        return await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => MapToResponse(u))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto)
    {
<<<<<<< HEAD
        var user = await _userRepo.GetByIdAsync(id.ToString());
=======
        var user = await _context.Users.FindAsync(id);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (user == null) return null;

        if (!string.IsNullOrEmpty(dto.FirstName)) user.FirstName = dto.FirstName;
        if (!string.IsNullOrEmpty(dto.LastName)) user.LastName = dto.LastName;
        if (!string.IsNullOrEmpty(dto.PhoneNumber)) user.PhoneNumber = dto.PhoneNumber;
        if (!string.IsNullOrEmpty(dto.Role)) user.Role = dto.Role;
        if (dto.Barangay != null) user.Barangay = dto.Barangay;
        if (dto.Municipality != null) user.Municipality = dto.Municipality;
        if (dto.Province != null) user.Province = dto.Province;
        user.UpdatedAt = DateTime.UtcNow;

<<<<<<< HEAD
        await _userRepo.UpdateAsync(id.ToString(), user);
=======
        await _context.SaveChangesAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return MapToResponse(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
<<<<<<< HEAD
        var user = await _userRepo.GetByIdAsync(id.ToString());
=======
        var user = await _context.Users.FindAsync(id);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (user == null) return false;

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
<<<<<<< HEAD
        await _userRepo.UpdateAsync(id.ToString(), user);
=======
        await _context.SaveChangesAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return true;
    }

    private static UserResponseDto MapToResponse(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            Barangay = user.Barangay,
            Municipality = user.Municipality,
            Province = user.Province,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}
