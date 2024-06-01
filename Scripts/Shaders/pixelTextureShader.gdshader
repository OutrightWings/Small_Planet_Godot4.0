shader_type spatial;

uniform sampler2D image;
uniform sampler2D filter : filter[_nearest, _linear][_mipmap][_anisotropic];

void vertex(){
    // float scale = texture(height,VERTEX.xz).a;
    // VERTEX.y = scale;
    // VERTEX.x *= scale;
    // VERTEX.z *= scale;
}
void fragment(){

    //vec4 color = texture(image,tex_position); // white color
    //ALBEDO = color.rgb;
}