shader_type spatial;

uniform sampler2D height;

void vertex(){
    float scale = texture(height,VERTEX.xz).a;
    VERTEX.y = scale;
    VERTEX.x *= scale;
    VERTEX.z *= scale;
}
void fragment(){
    vec4 color = vec4(19.0/255.0, 153.0/255.0, 12.0/255.0, 1.0); // white color
    ALBEDO = color.rgb;
}