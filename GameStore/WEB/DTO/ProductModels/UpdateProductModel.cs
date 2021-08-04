namespace GameStore.WEB.DTO.ProductModels
{
    public class UpdateProductModel : CreateProductModel
    {
        public int Id { get; set; }
    }
}

//i might not inherit from CreateProduct with immutable props such as Creation date and so on, but I thought it might be much more faster to set which props shouldn't be changed in UpdateMethod, than creating a new model, if there will be need to add some new props