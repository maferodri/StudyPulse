// ASP.NET Core serializa en camelCase por defecto (System.Text.Json)
export interface Activity {
    id: number
    nombre: string
}

// Contrato de escritura / entrada (POST /api/Activity)
export interface CreateActivityDto {
    nombre: string
}

//Contrato PUT
export interface UpdateActivityDto {
    nombre: string
}

// Contrato de error de FluentValidation
export interface FluentValidationError {
    propertyName: string
    errorMessage: string
    attemptedValue?: unknown
    errorCode?: string
}
