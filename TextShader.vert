#version 330 core
layout(location = 0) in vec3 position;
layout(location = 1) in vec2 textureCoords;
out vec2 TexCoord;
uniform mat4 projection;
uniform mat4 model;

void main(){
	gl_Position = projection*model*vec4(position,1.0f); // projection*view*
	TexCoord = vec2(textureCoords.x,textureCoords.y);
}