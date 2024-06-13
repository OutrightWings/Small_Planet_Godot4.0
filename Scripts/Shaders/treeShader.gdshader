shader_type spatial;

void vertex(){
    float scale = INSTANCE_CUSTOM.a;
    VERTEX.y *= scale;
    VERTEX.x *= scale;
    VERTEX.z *= scale;
    COLOR = INSTANCE_CUSTOM;
}

void fragment(){
    vec4 color = COLOR;
    color.a = 1.0;
    ALBEDO = color.rgb;
}