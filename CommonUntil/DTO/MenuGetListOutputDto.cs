
using CommonUntil.Enum;
using System;

namespace CommonUntil.DTO
{
    public class MenuGetListOutputDto 
    {
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public Guid? CreatorId { get; set; }
        public bool State { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public MenuTypeEnum MenuType { get; set; } = MenuTypeEnum.Menu;
        public string PermissionCode { get; set; }
        public Guid ParentId { get; set; }
        public string MenuIcon { get; set; }
        public string Router { get; set; }
        public bool IsLink { get; set; }
        public bool IsCache { get; set; }
        public bool IsShow { get; set; } = true;
        public string Remark { get; set; }
        public string Component { get; set; }
        public string Query { get; set; }
        public string RouterName { get; set; }
        public int OrderNum { get; set; }
        public MenuSourceEnum MenuSource { get; set; } = MenuSourceEnum.Ruoyi;

        //public List<MenuEntity>? Children { get; set; }
    }
}
