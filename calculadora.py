import math

def mostrar_menu_principal():
    print("\n" + "="*50)
    print("         CALCULADORA CIENTÍFICA")
    print("="*50)
    print("1. Operações Básicas")
    print("2. Potência e Raízes")
    print("3. Trigonometria")
    print("4. Logaritmos")
    print("5. Números Complexos")
    print("0. Sair")
    print("="*50)

def menu_basicas():
    while True:
        print("\n--- OPERAÇÕES BÁSICAS ---")
        print("1. Adição")
        print("2. Subtração")
        print("3. Multiplicação")
        print("4. Divisão")
        print("0. Voltar")
        
        op = input("Escolha: ")
        
        if op == "0":
            break
        elif op in ["1","2","3","4"]:
            a = float(input("Digite o primeiro número: "))
            b = float(input("Digite o segundo número: "))
            
            if op == "1":
                print(f"Resultado: {a} + {b} = {a + b}")
            elif op == "2":
                print(f"Resultado: {a} - {b} = {a - b}")
            elif op == "3":
                print(f"Resultado: {a} × {b} = {a * b}")
            elif op == "4":
                if b == 0:
                    print("Erro: Divisão por zero!")
                else:
                    print(f"Resultado: {a} ÷ {b} = {a / b}")
        else:
            print("Opção inválida!")
        
        input("\nPressione Enter para continuar...")

def menu_potencia_raiz():
    while True:
        print("\n--- POTÊNCIA E RAÍZES ---")
        print("1. Potência (x^y)")
        print("2. Raiz Quadrada")
        print("3. Raiz Cúbica")
        print("0. Voltar")
        
        op = input("Escolha: ")
        
        if op == "0":
            break
        elif op == "1":
            a = float(input("Digite a base: "))
            b = float(input("Digite o expoente: "))
            print(f"Resultado: {a}^{b} = {a ** b}")
        elif op == "2":
            a = float(input("Digite o número: "))
            if a < 0:
                print("Erro: Raiz quadrada de número negativo não é real!")
            else:
                print(f"Resultado: √{a} = {math.sqrt(a)}")
        elif op == "3":
            a = float(input("Digite o número: "))
            if a < 0:
                print(f"Resultado: ∛{a} = -{(-a)**(1/3)}")
            else:
                print(f"Resultado: ∛{a} = {a**(1/3)}")
        else:
            print("Opção inválida!")
        
        input("\nPressione Enter para continuar...")

def menu_trigonometria():
    while True:
        print("\n--- TRIGONOMETRIA (ângulos em graus) ---")
        print("1. Seno")
        print("2. Cosseno")
        print("3. Tangente")
        print("4. Arco Seno (inverso)")
        print("5. Arco Cosseno (inverso)")
        print("0. Voltar")
        
        op = input("Escolha: ")
        
        if op == "0":
            break
        elif op in ["1","2","3"]:
            ang = float(input("Digite o ângulo em graus: "))
            rad = math.radians(ang)
            
            if op == "1":
                print(f"sen({ang}°) = {math.sin(rad):.4f}")
            elif op == "2":
                print(f"cos({ang}°) = {math.cos(rad):.4f}")
            elif op == "3":
                print(f"tan({ang}°) = {math.tan(rad):.4f}")
        elif op == "4":
            val = float(input("Digite o valor (entre -1 e 1): "))
            if -1 <= val <= 1:
                print(f"arcsen({val}) = {math.degrees(math.asin(val)):.2f}°")
            else:
                print("Erro: Valor fora do domínio (-1 a 1)")
        elif op == "5":
            val = float(input("Digite o valor (entre -1 e 1): "))
            if -1 <= val <= 1:
                print(f"arccos({val}) = {math.degrees(math.acos(val)):.2f}°")
            else:
                print("Erro: Valor fora do domínio (-1 a 1)")
        else:
            print("Opção inválida!")
        
        input("\nPressione Enter para continuar...")

def menu_logaritmos():
    while True:
        print("\n--- LOGARITMOS ---")
        print("1. Logaritmo Natural (ln)")
        print("2. Logaritmo Base 10 (log10)")
        print("3. Logaritmo em qualquer base")
        print("0. Voltar")
        
        op = input("Escolha: ")
        
        if op == "0":
            break
        elif op == "1":
            a = float(input("Digite o número: "))
            if a <= 0:
                print("Erro: Logaritmo de número não positivo!")
            else:
                print(f"ln({a}) = {math.log(a):.4f}")
        elif op == "2":
            a = float(input("Digite o número: "))
            if a <= 0:
                print("Erro: Logaritmo de número não positivo!")
            else:
                print(f"log10({a}) = {math.log10(a):.4f}")
        elif op == "3":
            a = float(input("Digite o número: "))
            base = float(input("Digite a base: "))
            if a <= 0 or base <= 0 or base == 1:
                print("Erro: Parâmetros inválidos!")
            else:
                print(f"log{int(base)}({a}) = {math.log(a, base):.4f}")
        else:
            print("Opção inválida!")
        
        input("\nPressione Enter para continuar...")

def menu_complexos():
    while True:
        print("\n--- NÚMEROS COMPLEXOS ---")
        print("1. Adição (a + bi) + (c + di)")
        print("2. Multiplicação")
        print("3. Módulo")
        print("4. Forma Polar")
        print("0. Voltar")
        
        op = input("Escolha: ")
        
        if op == "0":
            break
        elif op == "1":
            a = float(input("Parte real do primeiro: "))
            b = float(input("Parte imaginária do primeiro: "))
            c = float(input("Parte real do segundo: "))
            d = float(input("Parte imaginária do segundo: "))
            z1 = complex(a, b)
            z2 = complex(c, d)
            print(f"Resultado: {z1} + {z2} = {z1 + z2}")
        elif op == "2":
            a = float(input("Parte real do primeiro: "))
            b = float(input("Parte imaginária do primeiro: "))
            c = float(input("Parte real do segundo: "))
            d = float(input("Parte imaginária do segundo: "))
            z1 = complex(a, b)
            z2 = complex(c, d)
            print(f"Resultado: {z1} × {z2} = {z1 * z2}")
        elif op == "3":
            a = float(input("Parte real: "))
            b = float(input("Parte imaginária: "))
            z = complex(a, b)
            print(f"Módulo de {z} = {abs(z):.4f}")
        elif op == "4":
            a = float(input("Parte real: "))
            b = float(input("Parte imaginária: "))
            z = complex(a, b)
            modulo = abs(z)
            fase = math.degrees(math.atan2(b, a))
            print(f"Forma polar: {modulo:.4f} ∠ {fase:.2f}°")
        else:
            print("Opção inválida!")
        
        input("\nPressione Enter para continuar...")

def main():
    while True:
        mostrar_menu_principal()
        opcao = input("Escolha uma opção: ")
        
        if opcao == "0":
            print("\nSaindo... Obrigado por usar a calculadora!")
            break
        elif opcao == "1":
            menu_basicas()
        elif opcao == "2":
            menu_potencia_raiz()
        elif opcao == "3":
            menu_trigonometria()
        elif opcao == "4":
            menu_logaritmos()
        elif opcao == "5":
            menu_complexos()
        else:
            print("Opção inválida!")
            input("Pressione Enter para continuar...")

if __name__ == "__main__":
    main()
    input("\nPressione Enter para sair...")
